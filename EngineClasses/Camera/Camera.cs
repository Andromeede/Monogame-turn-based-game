using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Channels;

namespace RPGWithManagers
{
    public class Camera
    {
        public readonly static float MinZ = 1f;
        public readonly static float MaxZ = 2048f;

        public readonly static float MinZoom = 0.25f;
        public readonly static int MaxZoom = 20;

        private Vector2 position, targetPosition, initialPos;
        private Vector2 screenCenterOffset2d = new Vector2(GlobalUtil.screenWidth / 2.0f, GlobalUtil.screenHeight / 2.0f);
        private Vector3 screenCenterOffset = new Vector3(GlobalUtil.screenWidth / 2.0f, GlobalUtil.screenHeight / 2.0f, 0);
        private float z;
        private float baseZ;

        private float zoom;
        private float cameraSpeed;
        private float zoomSpeed;
        private float shakeMovement;
        private float rotationAngleY;
        private float rotationAngleZ;
        private float rotationSpeed;
        private float rotationSpeedY;

        private float aspectRatio;
        private float fieldOfView;

        private Matrix view;
        private Matrix proj;
        private Matrix rView;
        private Matrix r2View;

        private MaTimer sTimer = new MaTimer(1000);

        private bool is3D;
        private bool startShake, verticalShake;
        private bool fCounterClockwise, fullRotation;

        private Queue<Action> queue = new Queue<Action>();
        private Action currentAction = null;

        private List<Vector2> path = new List<Vector2>();
        private List<float> zoomValues = new List<float>();
        private List<float> angleValues = new List<float>();
        private List<float> angleValuesY = new List<float>();


        #region Accessors
        public Vector2 Position
        {
            get { return position; }
        }
        public float Z
        {
            get { return z; }
        }

        public float Zoom
        {
            get { return zoom; }
        }

        public Matrix View
        {
            get { return view; }
        }

        public Matrix Rota2View
        {
            get { return r2View; }
        }

        public Matrix RotaView
        {
            get { return rView; }
        }

        public Matrix Projection
        {
            get { return proj; }
        }

        public float CameraSpeed
        {
            get { return cameraSpeed; }
        }

        public float ZoomSpeed
        {
            get { return zoomSpeed; }
        }
        #endregion
        public Camera(Vector2 ScreenSize, bool Is3D)
        {
            position = new Vector2(0, 0);
            is3D = Is3D;
            aspectRatio = (float)ScreenSize.X / ScreenSize.Y;
            fieldOfView = MathHelper.PiOver2;

            baseZ = GetZfromHeight(ScreenSize.Y);
            z = baseZ;
            zoom = 1.0f;
            cameraSpeed = 1.0f;
            zoomSpeed = .1f;        

            fCounterClockwise = fullRotation = false;

            rotationAngleY = 0.0f;
            rotationAngleZ = 0.0f;       
            rotationSpeed = 1.0f;
            rotationSpeedY = .2f;
            UpdateMatrices();

        }

        public virtual void UpdateMatrices()
        {
                                        
            if (is3D) 
            {
                Calculate3DTransform();
            }
            else
            {
                Calculate2DTransform();
            }

            if (queue.Count > 0 && currentAction == null)
            {
                currentAction = queue.Dequeue(); // dequeues the first function and retrieves it as an action 
                currentAction(); // calls the action (the stored function)        
            }

            if (rotationAngleY < 0)
            {
                rotationAngleY += 360;
            }

            rotationAngleY %= 360;

            if (!fullRotation)
            {
                if (rotationAngleZ < 0)
                {
                    rotationAngleZ += 360;
                }

                rotationAngleZ %= 360;
            }
          
            HandleShaking();
            HandleMovement();
            HandleZoom();
            HandleRotation();
            HandleRotationY();
        }

        public virtual void Calculate2DTransform()
        {
            // offset positions to look at the center of the screen 
            Vector2 offset = new Vector2(((GlobalUtil.screenWidth / zoom) - GlobalUtil.screenWidth) / 2.0f, ((GlobalUtil.screenHeight / zoom) - GlobalUtil.screenHeight) / 2.0f);

            float radians = MathHelper.ToRadians(-rotationAngleZ);//No axis correction
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            // handles offset after rotating around the Z -axis
            Vector2 rotatedOffset = new Vector2(offset.X * cos - offset.Y * sin, offset.X * sin + offset.Y * cos);

            // sets the view with the target point at the center of the screen
            view = Matrix.CreateLookAt(new Vector3(position.X, position.Y, z), new Vector3(position.X, position.Y, 0), Vector3.Up) * Matrix.CreateTranslation(rotatedOffset.X, rotatedOffset.Y, 0);
            rView = view * Matrix.CreateTranslation(-screenCenterOffset) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationAngleZ)) * Matrix.CreateTranslation(screenCenterOffset);
            //// handles rotation, a translation back to the camera center is needed to perform a ration to the camera center, then shifts everyting back
            proj = Matrix.CreateOrthographicOffCenter(0, GlobalUtil.screenWidth / zoom, GlobalUtil.screenHeight / zoom, 0, MinZ, MaxZ);
            // creates the actual screen projection and the space the objects will be in with origin in the top left of the screen and X facing right and Y facing downwards
        }

        public virtual void Calculate3DTransform()
        {
            // offset positions to look at the center of the screen 
            Vector3 offset3 = new Vector3(((GlobalUtil.screenWidth / zoom) - GlobalUtil.screenWidth) / 2.0f, ((GlobalUtil.screenHeight / zoom) - GlobalUtil.screenHeight) / 2.0f, 0);

            float radians = MathHelper.ToRadians(rotationAngleZ); //The Matrix.CreateScale in the proj matrix hadles the flip of the Y axis, so need to correct the rotation angle    
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            // handles offset after rotating around the Z -axis
            Vector3 rotatedOffset3 = new Vector3(offset3.X * cos - offset3.Y * sin, offset3.X * sin + offset3.Y * cos, offset3.Z);
            // base for 3D concepts, sets the view with the target point at the center of the screen 
            view = Matrix.CreateLookAt(new Vector3(position.X, position.Y, z), new Vector3(position.X, position.Y, 0), Vector3.Up);
            rView = view * Matrix.CreateTranslation(-screenCenterOffset / zoom) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotationAngleZ)) * Matrix.CreateTranslation(rotatedOffset3) * Matrix.CreateRotationY(MathHelper.ToRadians(rotationAngleY));
            //// Handles the correction of the zoom, rotates the view around the Z-axis, corrects the zoom offset after rotation
            proj = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, MinZ, MaxZ) * Matrix.CreateScale(new Vector3(1, -1, 1));
            //Creates the projection, in this case a 3D right-handed coordintate system and we flip the Y-axis to have the correct projection
        }

        public virtual void CameraShakeHorizontal(int Duration, float HorizontalMovement, float speed)
        {
            verticalShake = false;
            cameraSpeed = speed;
            shakeMovement = HorizontalMovement;
            initialPos = new Vector2(position.X,position.Y);
            startShake = true;
            sTimer = new MaTimer(Duration);
            targetPosition = new Vector2(position.X + shakeMovement, position.Y);
        }

        public virtual void CameraShakeVertical(int Duration, float HorizontalMovement, float speed)
        {
            verticalShake = true;
            cameraSpeed = speed;
            shakeMovement = HorizontalMovement;
            initialPos = new Vector2(position.X, position.Y);
            startShake = true;
            sTimer = new MaTimer(Duration);
            targetPosition = new Vector2(position.X , position.Y + shakeMovement);
        }

        public virtual float CorrectZoomValueFromDistance(float CameraSpeed, float TargetDistance, float CorrectiveZoomValue)
        {
            if (TargetDistance == 0)
            {
                return CameraSpeed;
            }
            else
            {
                return (CameraSpeed / TargetDistance) * CorrectiveZoomValue;
            }                  
        }

        public virtual float CorrectZoomValueFromRotation(float RotationSpeed, float RotationValue, float CorrectiveZoomValue)
        {
            if (RotationValue == 0)
            {
                return RotationSpeed;
            }
            else
            {
                return (RotationSpeed / RotationValue) * CorrectiveZoomValue;
            }
        }

        public virtual float CorrectRotationValueFromDistance(float CameraSpeed, float TargetDistance, float CorrectiveRotationValue)
        {
            if (TargetDistance == 0)
            {
                return CameraSpeed;
            }
            else
            {
                return (CameraSpeed / TargetDistance) * CorrectiveRotationValue;
            }
        }

        public virtual float CorrectRotationValueFromZoom(float ZoomSpeed, float ZoomDistance, float CorrectiveRotationValue)
        {
            if (ZoomDistance == 0)
            {
                return ZoomSpeed;
            }
            else
            {
                return (ZoomSpeed / ZoomDistance) * CorrectiveRotationValue;
            }
        }

        public virtual float CheckCounterClockwise(float AngleValue, bool IsCounterClockwise)
        {        
            if (IsCounterClockwise)
            {
                AngleValue = 360 - AngleValue;
            }

            return AngleValue;
        }

        public virtual void DecZoom()
        {
            if (zoom > 1)
            {
                zoom--;
            }
            else
            {
                zoom = zoom / 2;
            }
            zoom = MathHelper.Clamp(zoom, (int)MinZoom, MaxZoom);
            z = baseZ / zoom;

        }

        public virtual void DecZoomSmooth()
        {
            if (zoom > 1)
            {
                float newZoom = zoom - 1.0f; // can't use zoom -- as we need a float value
                SetZoomSmooth(newZoom, zoomSpeed);
            }
            else
            {
                float newZoom = zoom / 2;
                SetZoomSmooth(newZoom, zoomSpeed);
            }
        }

        public virtual void DirectRotationSide(float RotationValue, bool IsCounterClockwise)
        {
            if (IsCounterClockwise)
            {
                RotateZCounterClockWise(RotationValue);
            }
            else
            {
                RotateZClockWise(RotationValue);
            }
        }
    
        public virtual void HandleMovement()
        {
            if (path.Count > 0)
            {               
                //process movement logic
                if (path[0] != position) // if we have not reached target position
                {
                    ProcessMovements();
                }
                else
                {
                    path.RemoveAt(0);
                    ProcessMovements();
                    currentAction = null;
                }
                
            }
        }

        public virtual void HandleRotation()
        {
            if (angleValues.Count > 0)
            {
                if (angleValues[0] != rotationAngleZ) // if we have not reached target position
                {
                    ProcessRotation();
                }
                else
                {
                    angleValues.RemoveAt(0);
                    ProcessRotation();
                    fCounterClockwise = false;
                    fullRotation = false;
                    ResetRotationSpeedZ();
                }               
            }
        }

        public virtual void HandleRotationY()
        {
            if (angleValuesY.Count > 0)
            {
                if (angleValuesY[0] != rotationAngleY) // if we have not reached target position
                {
                    ProcessRotationY();
                }
                else
                {
                    angleValuesY.RemoveAt(0);
                    ProcessRotationY();

                    ResetRotationSpeedY();
                }
            }
        }

        public virtual void HandleShaking()
        {
            if (startShake)
            {
                sTimer.UpdateTimer();

                if (position == targetPosition)
                {
                    if (!verticalShake)
                    {                
                        if (path.Count == 0)
                        {
                            SetChangePosSmoothWithoutOffset(new Vector2(targetPosition.X - 2 * shakeMovement, targetPosition.Y));
                            ProcessMovements();                           
                        }                      
                    }
                    else
                    {                                           
                        if (path.Count == 0)
                        {
                            SetChangePosSmoothWithoutOffset(new Vector2(targetPosition.X, targetPosition.Y - 2 * shakeMovement));
                            ProcessMovements();
                        }                      
                    }
                }
                else
                {
                    if (path.Count == 0)
                    {
                        SetChangePosSmoothWithoutOffset(targetPosition);
                        ProcessMovements();
                    }                
                }
            }

            if (sTimer.Test())
            {
                startShake = false;
                path.Clear();
                SetDirectPos(initialPos);
                sTimer.ResetToZero();
                ResetCameraSpeed();
            }
        }

        public virtual void HandleZoom()
        {
            if (zoomValues.Count > 0)
            {
                
                if (zoomValues[0] != zoom)// if we have not reached target zoom
                {
                    ProcessZoom();
                }
                else
                {                     
                    zoomValues.RemoveAt(0);
                    ProcessZoom();
                    ResetZoomSpeed();
                    currentAction = null;
                }
                
            }
        }

        public virtual void IncZoom()
        {
            if (zoom > 1)
            {
                zoom++;
            }
            else
            {
                zoom = zoom * 2;
            }
            zoom = MathHelper.Clamp(zoom, (int)MinZoom, MaxZoom);
            z = baseZ / zoom;
        }

        public virtual void IncZoomSmooth()
        {
            if (zoom > 1)
            {
                float newZoom = zoom + 1.0f; // can't use zoom ++ as we need a float value
                SetZoomSmooth(newZoom, zoomSpeed);
            }
            else
            {
                float newZoom = zoom * 2;
                SetZoomSmooth(newZoom, zoomSpeed);
            }
        }
        public virtual void GetExtents(out float Width, out float Height)
        {
            Height = GetHeightFromZ();
            Width = Height * aspectRatio;
        }

        public virtual void GetExtents(out float Left, out float Right, out float Bottom, out float Top)
        {
            GetExtents(out float Width, out float Height);
            //top left origin
            Left = (GlobalUtil.screenWidth - GlobalUtil.screenWidth / zoom) / 2.0f;
            Right = Width + Left;
            Top = (GlobalUtil.screenHeight - GlobalUtil.screenHeight / zoom) / 2.0f;
            Bottom = Height + Top;
        }

        public virtual void GetExtents(out Vector2 Min, out Vector2 Max)
        {
            GetExtents(out float Left, out float Right, out float Bottom, out float Top);
            Min = new Vector2(Left, Top);
            Max = new Vector2(Right, Bottom);
        }     

        public virtual float GetHeightFromZ()
        {
            return z * MathF.Tan(0.5f * fieldOfView) * 2.0f;
        }

        public virtual float GetZfromHeight(float Height)
        {
            return (0.5f * Height) / MathF.Tan(0.5f * fieldOfView);
        }

        public virtual void ProcessMovements()
        {
            if (path.Count != 0)
            {
                Vector2 change = GlobalUtil.RadialMovement(path[0], position, cameraSpeed);
                SetChangePos(change);
            }
        }

        public virtual void ProcessRotation()
        {
            if (angleValues.Count != 0)
            {
                float change = Math.Abs(GlobalUtil.RadialAbsFloat(angleValues[0], rotationAngleZ, rotationSpeed));

                if (angleValues[0] != rotationAngleZ)
                {
                    if (fCounterClockwise)
                    {
                        RotateZCounterClockWise(change);
                    }
                    else
                    {
                        RotateZClockWise(change);
                    }
                }                                            
            }
        }

        public virtual void ProcessRotationY()
        {
            if (angleValuesY.Count != 0)
            {
                float change = Math.Abs(GlobalUtil.RadialAbsFloat(angleValuesY[0], rotationAngleY, rotationSpeedY));

                if (fCounterClockwise)
                {
                    RotateYCounterClockWise(change);
                }
                else
                {
                    RotateYClockWise(change);
                }
            }
        }

        public virtual void ProcessZoom()
        {
            if (zoomValues.Count != 0)
            {
                float change = GlobalUtil.RadialAbsFloat(zoomValues[0], zoom, zoomSpeed);
                SetZoom(change);
            }
        }

        public virtual void RotateYClockWise(float RotationValue)
        {
            rotationAngleY += RotationValue;
        }

        public virtual void RotateYCounterClockWise(float RotationValue)
        {
            rotationAngleY -= RotationValue; 
        }

        public virtual void RotateZClockWise(float RotationValue) 
        {
            rotationAngleZ += RotationValue;
        }

        public virtual void RotateZCounterClockWise(float RotationValue)
        {
            rotationAngleZ -= RotationValue;
        }

        public virtual void RotateFullCircle(int NumberOfTurns , bool IsCounterClockwise)
        {
            fullRotation = true;
            if (IsCounterClockwise)
            {
                SetRotationSmooth(rotationAngleZ - 360 * NumberOfTurns, rotationSpeed ,IsCounterClockwise);
            }
            else
            {
                SetRotationSmooth(rotationAngleZ + 360 * NumberOfTurns, rotationSpeed, IsCounterClockwise);
            }
            
        }

        public virtual void SetChangePos(Vector2 Amount)
        {
            position += Amount;
        }

        public virtual void SetDirectPos(Vector2 Amount)
        {
            position = Amount;
            currentAction = null; // resets the action in case of a queue of SetDirectPos()
        }

        public virtual void SetDirectPosWithOffset(Vector2 Amount, Vector2 PosOffset)
        {
            SetDirectPos(Amount - PosOffset);
        }

        public virtual void SetDirectRotationZ(float RotationValue)
        {
            rotationAngleZ = RotationValue;
        }

        public virtual void SetDirectPosWithRotation(Vector2 Pos, float RotationValue) //direct position with direct movement
        {
            ResetCameraValues();
            SetDirectPosWithOffset(Pos, screenCenterOffset2d); // needs offsets as it will move with the point in the top left cormer
            SetDirectRotationZ(RotationValue); // direct rotation, no need to handle direction
        }

        public virtual void SetDirectPosWithSmoothRotation(Vector2 Pos, float RotationValue, float RotationSpeed, bool IsCounterClockwise) //direct position with smooth rotation
        {
            ResetCameraValues();
            SetDirectPosWithOffset(Pos, screenCenterOffset2d); // needs offsets as it will move with the point in the top left cormer
            SetRotationSmooth(RotationValue, RotationSpeed, IsCounterClockwise);
        } 

        public virtual void SetDirectPosWithZoom(Vector2 Pos, float ZoomValue)// direct position and direct zoom 
        {
            ResetCameraValues();
            SetDirectPosWithOffset(Pos, screenCenterOffset2d); // needs offsets as it will move with the point in the top left cormer
            SetZoomDirect(ZoomValue);
        }

        public virtual void SetDirectPosWithZoomAndRotation(Vector2 Pos, float ZoomValue, float RotationValue)// direct position with direct zoom and direct rotation
        {
            ResetCameraValues();
            SetDirectPosWithOffset(Pos, screenCenterOffset2d); // needs offsets as it will move with the point in the top left cormer
            SetZoomDirect(ZoomValue);
            SetDirectRotationZ(RotationValue); //direct rotation, no need to hadle direction
        }

        public virtual void SetDirectPosWithSmoothZoom(Vector2 Pos, float ZoomValue, float ZoomSpeed)// direct position with smooth zoom 
        {
            ResetCameraValues();
            SetDirectPosWithOffset(Pos, screenCenterOffset2d); // needs offsets as it will move with the point in the top left cormer
            SetZoomSmooth(ZoomValue, ZoomSpeed);
        }

        public virtual void SetChangePosSmooth(Vector2 TargetPos, float CameraSpeed)// smooth position
        {
            cameraSpeed = CameraSpeed;
            path.Clear();
            path.Add(TargetPos - screenCenterOffset2d);
        }

        public virtual void SetChangePosSmoothWithoutOffset(Vector2 TargetPos)// smooth position without offset
        {
            path.Clear();
            path.Add(TargetPos);
        } 

        public virtual void SetChangePosSmoothWithRotation(Vector2 Pos, float RotationValue, float CameraSpeed) //smooth position with direct rotation
        {          
            SetChangePosSmooth(Pos, CameraSpeed);
            SetDirectRotationZ(RotationValue); //direct rotation, no need to hadle direction
        }

        public virtual void SetChangePosSmoothWithSmoothRotation(Vector2 Pos, float RotationValue, float CameraSpeed, bool IsCounterClockwise) //smooth position with smooth rotation with rotation adjusted from camera speed
        {
            float targetDistance = Math.Abs(GlobalUtil.GetDistance(position, Pos - screenCenterOffset2d));
            float correctiveRotationValue = Math.Abs(rotationAngleZ - RotationValue);

            correctiveRotationValue = CheckCounterClockwise(correctiveRotationValue, IsCounterClockwise);
            rotationSpeed = CorrectRotationValueFromDistance(cameraSpeed, targetDistance, correctiveRotationValue);

            SetChangePosSmooth(Pos, CameraSpeed);
            SetRotationSmooth(RotationValue, rotationSpeed, IsCounterClockwise);

        }

        public virtual void SetChangePosSmoothWithSmoothZoom(Vector2 Pos, float CameraSpeed, float ZoomValue)// smooth position with smooth zoom 
        {
            float targetDistance = Math.Abs(GlobalUtil.GetDistance(position, Pos - screenCenterOffset2d));
            float correctiveZoomValue = Math.Abs(zoom - ZoomValue); // to adjust zoomspeed depending on the zoom difference of initial and targeted zoom

            zoomSpeed = CorrectZoomValueFromDistance(CameraSpeed, targetDistance, correctiveZoomValue);
            
            SetChangePosSmooth(Pos, CameraSpeed);
            SetZoomSmooth(ZoomValue, zoomSpeed);
        }

        public virtual void SetChangePosSmoothWithSmoothZoomAndSmoothRotation(Vector2 Pos, float CameraSpeed, float ZoomValue, float RotationValue, bool IsCounterClockwise) //smooth position with snooth zoom and smooth rotation
        { // with zoomspeed and rotation adjusted from cameraspeed         
            float targetDistance = Math.Abs(GlobalUtil.GetDistance(position, Pos - screenCenterOffset2d));
            float correctiveZoomValue = Math.Abs(zoom - ZoomValue); // to adjust zoomspeed depending on the zoom difference of initial and targeted zoom
            float correctiveRotationValue = Math.Abs(rotationAngleZ - RotationValue);

            correctiveRotationValue = CheckCounterClockwise(correctiveRotationValue, IsCounterClockwise);

            if (targetDistance == 0)
            {
                zoomSpeed = CorrectZoomValueFromRotation(CameraSpeed, correctiveRotationValue, correctiveZoomValue);
            }
            else
            {
                zoomSpeed = CorrectZoomValueFromDistance(CameraSpeed, targetDistance, correctiveZoomValue); //can be infinite if targetdistance is 0 !-> will return cameraspeed          
                rotationSpeed = CorrectRotationValueFromDistance(CameraSpeed, targetDistance, correctiveRotationValue);
            }

            SetChangePosSmooth(Pos, CameraSpeed);
            SetZoomSmooth(ZoomValue, zoomSpeed);
            SetRotationSmooth(RotationValue, rotationSpeed, IsCounterClockwise);
        }

        public virtual void SetChangePosSmoothThenSmoothZoom(Vector2 Pos, float CameraSpeed, float ZoomValue, float ZoomSpeed)// smooth position then smooth zoom 
        {
            queue.Clear();
            queue.Enqueue(() => SetChangePosSmooth(Pos, CameraSpeed));
            queue.Enqueue(() => SetZoomSmooth(ZoomValue, ZoomSpeed));
        }

        public virtual void SetChangePosSmoothThenZoom(Vector2 Pos, float CameraSpeed, float ZoomValue)// smooth position then direct zoom 
        {
            queue.Clear();
            queue.Enqueue(() => SetChangePosSmooth(Pos, CameraSpeed));
            queue.Enqueue(() => SetZoomDirect(ZoomValue));
        }

        public virtual void SetSmoothZoomThenSetChangePosSmooth(Vector2 Pos, float CameraSpeed, float ZoomValue, float ZoomSpeed)// smooth zoom then smooth position 
        {
            queue.Clear();
            queue.Enqueue(() => SetZoomSmooth(ZoomValue, ZoomSpeed));
            queue.Enqueue(() => SetChangePosSmooth(Pos, CameraSpeed));          
        }

        public virtual void SetSmoothZoomThenSetChangePos(Vector2 Pos, float ZoomValue, float ZoomSpeed)// smooth zoom then direct position 
        {         
            queue.Clear();
            queue.Enqueue(() => SetZoomSmooth(ZoomValue, ZoomSpeed));
            queue.Enqueue(() => SetDirectPos(Pos - screenCenterOffset2d));
        }

        public virtual void SetSmoothZoomWithRotation(float ZoomValue, float RotationValue, float RotationSpeed)// smooth zoom with direct rotation
        {
            SetZoomSmooth(ZoomValue, RotationSpeed);
            SetDirectRotationZ(RotationValue); //direct rotation, no need to hadle direction
        }

        public virtual void SetSmoothZoomWithSmoothRotation(float ZoomValue, float RotationValue, float RotationSpeed, bool IsCounterClockwise)// smooth zoom with smooth rotation with zoom speed adjusted from rotation
        {
            float correctiveZoomValue = Math.Abs(zoom - ZoomValue); // to adjust zoomspeed depending on the zoom difference of initial and targeted zoom
            float correctiveRotationValue = Math.Abs(rotationAngleZ - RotationValue);

            correctiveRotationValue = CheckCounterClockwise(correctiveRotationValue, IsCounterClockwise);

            zoomSpeed = CorrectZoomValueFromRotation(RotationSpeed, correctiveRotationValue, correctiveZoomValue);

            SetZoomSmooth(ZoomValue, zoomSpeed);
            SetRotationSmooth(RotationValue, RotationSpeed, IsCounterClockwise);
        }

        public virtual void SetRotationSmooth(float Amount, float RotationZspeed, bool IsCounterClockwise)// smooth rotation Z axis
        {
            fCounterClockwise = IsCounterClockwise;
            rotationSpeed = RotationZspeed;
            angleValues.Clear();
            angleValues.Add(Amount);                     
        }

        public virtual void SetRotationYSmooth(float Amount, float RotationYspeed , bool IsCounterClockwise)// smooth rotation Y axis
        {
            fCounterClockwise = IsCounterClockwise;
            rotationSpeedY = RotationYspeed;

            angleValuesY.Clear();
            angleValuesY.Add(Amount);
        }

        public virtual void SetZ(float Amount)
        {
            z += Amount;
            z = MathHelper.Clamp(z, MinZ, MaxZ);
        }

        public virtual void SetZoom(float ZoomValue)
        {
            zoom += ZoomValue;
            zoom = MathHelper.Clamp(zoom, (int)MinZoom, MaxZoom);
            z = baseZ / zoom;
        }

        public virtual void SetZoomDirect(float ZoomValue)
        {
            zoom = ZoomValue;
            zoom = MathHelper.Clamp(zoom, (int)MinZoom, MaxZoom);
            z = baseZ / zoom;
            currentAction = null;  // resets the action in case of a queue of SetZoomDirect()
        }

        public virtual void SetZoomThenSetChangePosSmooth(Vector2 Pos, float CameraSpeed, float ZoomValue)// direct zoom then smooth position 
        {
            queue.Clear();
            queue.Enqueue(() => SetZoomDirect(ZoomValue));
            queue.Enqueue(() => SetChangePosSmooth(Pos, CameraSpeed));
        }

        public virtual void SetZoomWithRotation(float ZoomValue, float RotationValue) // direct zoom with direct rotation
        {
            SetZoomDirect(ZoomValue);
            SetDirectRotationZ(RotationValue); 
        }

        public virtual void SetZoomWithSmoothRotation(float ZoomValue, float RotationValue, bool IsCounterClockwise) // direct zoom with smooth rotation
        {
            SetZoomDirect(ZoomValue);
            SetRotationSmooth(RotationValue, rotationSpeed, IsCounterClockwise);
        }

        public virtual void SetZoomSmooth(float Value, float ZoomSpeed)
        {
            zoomSpeed = ZoomSpeed;
            zoomValues.Clear();
            zoomValues.Add(Value);
        }

        public virtual void ResetCameraValues()
        {
            queue.Clear();
            SetDirectPos(new Vector2(0,0));
            SetZoomDirect(1);
            ResetCameraSpeed();           
            ResetZoomSpeed();
            ResetRotationY();
            ResetRotationZ();
            fullRotation = false;
        }    
        public virtual void ResetCameraSpeed()
        {
            cameraSpeed = 1.0f;
        }
        public virtual void ResetRotationSpeedY()
        {
            rotationSpeedY = 1.0f;
        }
        public virtual void ResetRotationSpeedZ()
        {
            rotationSpeed = 1.0f;
        }
        public virtual void ResetRotationY()
        {
            rotationAngleY = 0.0f;
        }
        public virtual void ResetRotationZ()
        {
            rotationAngleZ = 0.0f;
        }
        public virtual void ResetZ()
        {
            z = baseZ;
        }
        public virtual void ResetZoomSpeed()
        {
            zoomSpeed = .1f;
        }
    }
}
