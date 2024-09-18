using Share;
using Share.EmailService;
using Share.FileManagement;

namespace Swinva.share
{
    public class ShareConfigData
    {
        public EmailConfiguration? EMAIL_CONFIGURATION { get; set; }
    
        public ObjectStorageConfiguration? ObjectStorageConfiguration { get; set; }
      
        public DeploymentConfiguration? DEPLOYMENT_CONFIGURATION { get; set; }
    }
}
