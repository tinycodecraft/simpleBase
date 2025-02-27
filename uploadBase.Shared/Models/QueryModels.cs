﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace uploadBase.Shared.Models
{

    public class ExceptionDetails
    {
        public readonly int StatusCode;
        public readonly string Message;

        public ExceptionDetails(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message ?? "No error message found in exception.";
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }

    public class DomainException : Exception
    {
        public DomainException(string message, string code = null)
            : base(message)
        {
            Code = code;
        }

        public string Code { get; }
    }
    //using in react mantine table
    public class SortProps
    {
        public bool Desc { get; set; }

        public string Id { get; set; }

    }
    //using in react mantine table
    public class FilterProps
    {
        public string Id { get; set; }  
        public dynamic Value { get; set; }


    }
    //using in react mantine table
    public class DescProps
    {
        public string value { get; set; } = "";
        public string label { get; set; } = "";
    }

    //using in react mantine table
    public class MantineTableProps
    {
        public string Type { get; set; } = "";

        //record start index
        public int Start { get; set; }

        //page size
        public int Size { get; set; }

        public FilterProps[] Filtering { get; set; } = [];

        public string GlobalFilter { get; set; }= "";

        public SortProps[] Sorting { get; set; } = [];

        public bool? WithDisabled { get; set; }

        public string? SelectedIds { get; set; }
    }
    

}
