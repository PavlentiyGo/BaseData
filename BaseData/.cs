using System;

namespace BaseData
{
    public class JoinCondition
    {
        public string FirstTable { get; set; } = string.Empty;
        public string SecondTable { get; set; } = string.Empty;
        public string FirstColumn { get; set; } = string.Empty;
        public string SecondColumn { get; set; } = string.Empty;
        public string JoinType { get; set; } = string.Empty;

        public JoinCondition()
        {
            // Конструктор по умолчанию для инициализации
        }

        public JoinCondition(string firstTable, string secondTable, string firstColumn, string secondColumn, string joinType)
        {
            FirstTable = firstTable ?? string.Empty;
            SecondTable = secondTable ?? string.Empty;
            FirstColumn = firstColumn ?? string.Empty;
            SecondColumn = secondColumn ?? string.Empty;
            JoinType = joinType ?? "INNER JOIN";
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(FirstTable) || string.IsNullOrEmpty(SecondTable) ||
                string.IsNullOrEmpty(FirstColumn) || string.IsNullOrEmpty(SecondColumn) ||
                string.IsNullOrEmpty(JoinType))
            {
                return "Неполное условие соединения";
            }

            return $"{FirstTable}.{FirstColumn} {JoinType} {SecondTable}.{SecondColumn}";
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstTable) &&
                   !string.IsNullOrEmpty(SecondTable) &&
                   !string.IsNullOrEmpty(FirstColumn) &&
                   !string.IsNullOrEmpty(SecondColumn) &&
                   !string.IsNullOrEmpty(JoinType);
        }

        public string ToSqlCondition()
        {
            if (!IsValid())
            {
                throw new InvalidOperationException("Невозможно сформировать SQL условие: не все поля заполнены");
            }

            return $"{FirstTable}.{FirstColumn} = {SecondTable}.{SecondColumn}";
        }

        public string ToJoinClause()
        {
            if (!IsValid())
            {
                throw new InvalidOperationException("Невозможно сформировать JOIN clause: не все поля заполнены");
            }

            return $"{JoinType} {SecondTable} ON {FirstTable}.{FirstColumn} = {SecondTable}.{SecondColumn}";
        }

        public override bool Equals(object obj)
        {
            if (obj is JoinCondition other)
            {
                return FirstTable == other.FirstTable &&
                       SecondTable == other.SecondTable &&
                       FirstColumn == other.FirstColumn &&
                       SecondColumn == other.SecondColumn &&
                       JoinType == other.JoinType;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                FirstTable?.GetHashCode() ?? 0,
                SecondTable?.GetHashCode() ?? 0,
                FirstColumn?.GetHashCode() ?? 0,
                SecondColumn?.GetHashCode() ?? 0,
                JoinType?.GetHashCode() ?? 0
            );
        }

        public JoinCondition Clone()
        {
            return new JoinCondition
            {
                FirstTable = this.FirstTable,
                SecondTable = this.SecondTable,
                FirstColumn = this.FirstColumn,
                SecondColumn = this.SecondColumn,
                JoinType = this.JoinType
            };
        }
    }
}