﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Contracts.Enumerations
{
    public enum EncryptionMethod
    {
        /// <summary>
        /// One way encryption of data, this form of encryption can not be decrypted.
        /// </summary>
        Hashing = 1,
        /// <summary>
        /// Two way encryption, values encrypted can be decrypted.
        /// </summary>
        Encryption = 2
    }
}
