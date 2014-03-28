using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using HomeOS.Hub.Platform.Views;

namespace HomeOS.Hub.Apps.CameraImager
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class CameraImagerSvc : ISmartCamContract
    {
        protected VLogger logger;
        CameraImager camera;

        public CameraImagerSvc(VLogger logger, CameraImager smartCam)
        {
            this.logger = logger;
            camera = smartCam;
        }

        public string GetWebImage(string cameraFriendlyName)
        {
            try
            {
                byte[] image = camera.GetImage(cameraFriendlyName);
                return Convert.ToBase64String(image, 0, image.Length, Base64FormattingOptions.InsertLineBreaks);
            }
            catch (Exception e)
            {
                logger.Log("Got exception in GetWebImage: " + e);
                return String.Empty;
            }
        }

        public List<string> ControlCamera(string control, string cameraFriendlyName)
        {
            try
            {
                string controlType = null;

                control = control.ToLower();

                switch (control)
                {
                    case "down":
                        controlType = Common.RolePTCamera.OpDownName;
                        break;
                    case "up":
                        controlType = Common.RolePTCamera.OpUpName;
                        break;
                    case "left":
                        controlType = Common.RolePTCamera.OpLeftName;
                        break;
                    case "right":
                        controlType = Common.RolePTCamera.OpRightName;
                        break;
                    case "zoomout":
                        controlType = Common.RolePTZCamera.OpZommOutName;
                        break;
                    case "zoomin":
                        controlType = Common.RolePTZCamera.OpZoomInName;
                        break;
                    default:
                        logger.Log("unknown camera command");
                        break;
                }

                if (controlType != null)
                    camera.SendMsgToCamera(controlType, cameraFriendlyName);

                return new List<string>() { "" };
            }
            catch (Exception e)
            {
                logger.Log("Got exception in ControlCamera: " + e);
                return new List<string>() { e.Message };
            }
        }

        public List<string> GetRecordedClips(string cameraFriendlyName, int countMax)
        {
            try
            {

                string[] clips = camera.GetRecordedClips(cameraFriendlyName, countMax);

                List<string> retList = new List<string>() { "" };

                retList.AddRange(clips);

                return retList;
            }
            catch (Exception ex)
            {
                logger.Log("Exception in GetRecordedClips({0}, {1}): {2}", cameraFriendlyName, countMax.ToString(), ex.ToString());
                return new List<string>() { "Exception: " + ex.Message };
            }
        }

        //returns [cameraName, roleName] pairs
        public List<string> GetCameraList()
        {
            try
            {
                List<string> retList = new List<string>() { "" };
                retList.AddRange(camera.GetCameraList());
                return retList;
            }
            catch (Exception e)
            {
                logger.Log("Got exception in GetCameraList2: " + e);
                return new List<string>() { e.Message };
            }
        }

        public List<string> GetRecordedClipsCount(string cameraFriendlyName)
        {
            try 
            {
                int count = camera.GetRecordedClipsCount(cameraFriendlyName);
                return new List<string>() { "", count.ToString() };
            }
            catch (Exception e)
            {
                logger.Log("Got exception in GetRecordedClipsCount for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() {e.Message};
            }
        }

        public List<string> GetRecordedCamerasList()
        {
            try
            {
                string[] clips = camera.GetRecordedCamerasList();

                List<string> retList = new List<string>() { "" };

                retList.AddRange(clips);

                return retList;
            }
            catch (Exception ex)
            {
                logger.Log("Exception in GetRecordedCamerasList: {0}", ex.ToString());
                return new List<string>() { "Exception: " + ex.Message };
            }
        }


        public List<string> StartOrContinueRecording(string cameraFriendlyName)
        {
            try
            {
                camera.StartOrContinueRecording(cameraFriendlyName);
                return new List<string>() { "" };
            }
            catch (Exception e)
            {
                logger.Log("Exception in startorcontinuerecording for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };
            }
        }


        /// <summary>
        /// Called from UI to stop recording immediately
        /// </summary>
        /// <param name="cameraFriendlyName"></param>
        /// <returns></returns>
        public List<string> StopRecording(string cameraFriendlyName)
        {
            try
            {
                camera.StopRecording(cameraFriendlyName);

                return new List<string>() { "" };
            }
            catch (Exception e)
            {
                logger.Log("Exception in startorcontinuerecording for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };
            }
        }


        /// <summary>
        /// Sets (or turns off) Motion triggered recording for a camera
        /// </summary>
        /// <param name="cameraFriendlyName"></param>
        /// <param name="enable"></param>
        public List<string> EnableMotionTrigger(string cameraFriendlyName, bool enable)
        {
            try
            {

                camera.EnableMotionTrigger(cameraFriendlyName, enable);
                return new List<string>() { "" };
            }
            catch (Exception e)
            {
                logger.Log("Exception in CameraImager:EnableMotionTrigger for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };
            }
        }


        /// <summary>
        /// Returns whether motion triggered recording is enabled
        /// </summary>
        /// <param name="cameraFriendlyName"></param>
        /// <returns></returns>
        public List<string> IsMotionTriggerEnabled(string cameraFriendlyName)
        {
            try
            {
                bool result = camera.IsMotionTriggerEnabled(cameraFriendlyName);
                List<string> retList = new List<string>() { "" , result.ToString() };
                return retList;

                
            }
            catch (Exception e)
            {
                logger.Log("Exception in CameraImager:IsMotionTriggerEnabled for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };

            }
        }


        /// <summary>
        /// Sets (or turns off) Motion triggered recording for a camera
        /// </summary>
        /// <param name="cameraFriendlyName"></param>
        /// <param name="enable"></param>
        public List<string> EnableVideoUpload(string cameraFriendlyName, bool enable)
        {
            try
            {

                camera.EnableVideoUpload(cameraFriendlyName, enable);
                return new List<string>() { "" };
            }
            catch (Exception e)
            {
                logger.Log("Exception in CameraImager:EnableMotionTrigger for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };
            }
        }


        /// <summary>
        /// Returns whether motion triggered recording is enabled
        /// </summary>
        /// <param name="cameraFriendlyName"></param>
        /// <returns></returns>
        public List<string> IsVideoUploadEnabled(string cameraFriendlyName)
        {
            try
            {
                bool result = camera.IsVideoUploadEnabled(cameraFriendlyName);
                List<string> retList = new List<string>() { "", result.ToString() };
                return retList;


            }
            catch (Exception e)
            {
                logger.Log("Exception in CameraImager:IsVideoUploadEnabled for {0}: {1}", cameraFriendlyName, e.ToString());
                return new List<string>() { "Got exception: " + e.Message };

            }
        }

    }


    [ServiceContract]
    public interface ISmartCamContract
    {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetWebImage(string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<string> ControlCamera(string control, string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> GetCameraList();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> StartOrContinueRecording(string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> StopRecording(string cameraFriendlyName);

        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        //void RecordVideo(string cameraFriendlyName, bool enable);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> EnableMotionTrigger(string cameraFriendlyName, bool enable);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> IsMotionTriggerEnabled(string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> EnableVideoUpload(string cameraFriendlyName, bool enable);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> IsVideoUploadEnabled(string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        List<string> GetRecordedCamerasList();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<string> GetRecordedClipsCount(string cameraFriendlyName);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<string> GetRecordedClips(string cameraFriendlyName, int countMax);

        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //int GetClipTriggerImagesCount(string clipUrl);

        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //string[] GetClipTriggerImages(string clipUrl, int countMax);

        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //void SendEmail(string toAddress, string subject, string body, string[] attachmentUrls);
    }          
}
