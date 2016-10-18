namespace Script.I200.Entity.Api.Shared
{
    public class UploadCalculationInfo
    {

        public YouPaiImgParams Payload { get; set; }

        public bool HasBusinessLicense { get; set; }
    }

    public class YouPaiImgParams
    {
        public string Policy { get; set; }

        public string Signature { get; set; }

        public string Prefix { get; set; }

        public string BucketName { get; set; }
    }
}