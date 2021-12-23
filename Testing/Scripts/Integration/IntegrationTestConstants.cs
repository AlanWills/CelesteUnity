namespace Celeste.Testing
{
    public static class IntegrationTestConstants
    {
        // Scene Names
        public static readonly string INTEGRATION_TEST_SCENE_PATH = $"Assets/Celeste/Testing/Scenes/{INTEGRATION_TEST_SCENE_NAME}.unity";
        public const string INTEGRATION_TEST_SCENE_NAME = "IntegrationTestBootstrap";

        // Fail Reasons
        public const string FAIL_REASON_MISSING_SNAPSHOT = "Missing Snapshot";
        public const string FAIL_REASON_ERROR_DURING_SETUP = "Error During Setup";
        public const string FAIL_REASON_LOG_MESSAGE_TYPE = "Log Message Type";
        public const string FAIL_REASON_TIMEOUT = "Timeout";
        public const string FAIL_REASON_CONDITION = "Expected Results not matching Actual Results";
    }
}