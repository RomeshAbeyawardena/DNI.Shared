﻿namespace DNI.Core.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IParameterSchema
    {
        Type Type { get; set; }

        string Format { get; set; }
    }
}
