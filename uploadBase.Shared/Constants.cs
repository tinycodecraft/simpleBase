namespace uploadBase.Shared
{

    public class Constants
    {

        public enum FieldType
        {

        }

        public static class Op
        {

            public const string equal = "eq";
            public const string greaterThanOrEqual = "gte";
            public const string lessThanOrEqual = "lte";
            public const string lessThan = "lt";
            public const string itLikes = "ct";
            public const string Between = "bw";
            public const string BeginWith = "bt";
            public const string EndWith = "et";
            public const string Within = "in";
            public const string CheckListIn = "at";
        }
        public enum QueryOpType
        {
            Equal,
            StartsWith,
            EndsWith,
            ContainsWith,
            LikesWith,
            NotEq,
            GreaterOrEq,
            LessOrEq,
            Less,
            InListOp,
            OrderBy,
            ThenBy,
        }

        public static class Setting
        {
            public const string PathSetting = nameof(PathSetting);
            public const string AuthSetting = nameof(AuthSetting);
            public const string TemplateSetting = nameof(TemplateSetting);
            public const string CorsPolicySetting = nameof(CorsPolicySetting);

        }

    }
}
