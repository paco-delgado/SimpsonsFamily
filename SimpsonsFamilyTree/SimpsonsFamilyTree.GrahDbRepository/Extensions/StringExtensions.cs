using System;

namespace SimpsonsFamilyTree.Repository.Neo4j.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ToDate(this string stringDate)
        {
            IFormatProvider culture = new System.Globalization.CultureInfo("en-US");
            return Convert.ToDateTime(stringDate, culture);
        }

        public static string ToCommonName(this string relation)
        {
            switch (relation)
            {
                case "IS_PARENT_OF":
                    return "Parent";
                case "IS_CHILD_OF":
                    return "Child";
                case "IS_SIBILING_OF":
                    return "Sibiling";
                case "IS_PARTNER_OF":
                    return "Partner";
                default:
                    return string.Empty;
            }
        }
    }
}
