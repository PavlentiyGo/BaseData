namespace BaseData
{
    public class JoinCondition
    {
        public string FirstTable { get; set; }
        public string SecondTable { get; set; }
        public string FirstColumn { get; set; }
        public string SecondColumn { get; set; }
        public string JoinType { get; set; }

        public override string ToString()
        {
            return $"{FirstTable}.{FirstColumn} {JoinType} {SecondTable}.{SecondColumn}";
        }
    }
}