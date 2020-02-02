﻿using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Services.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace DNI.Shared.App.Domains
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [DefaultValue]
        public Guid UniqueId { get; set; }

        [Encrypt(Constants.IdentifierDataEncryption)]
        public byte[] EmailAddress { get; set; }
        
        [Encrypt(Constants.PersonalDataEncryption)]
        public byte[] FirstName { get; set; }
        
        [Encrypt(Constants.PersonalDataEncryption)]
        public byte[] MiddleName { get; set; }
        
        [Encrypt(Constants.PersonalDataEncryption)]
        public byte[] LastName { get; set; }
        
        [Modifier(ModifierFlag.Created)]
        public DateTime? Created { get; set; }

        [Modifier(ModifierFlag.Created | ModifierFlag.Modified)]
        public DateTimeOffset? Modified { get; set; }
    }
}
