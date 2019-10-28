using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyXSpace.Web.ViewModels
{
    public class LoginViewModel
    {
        /// <summary>
        /// TODO: this is Tenant\Company where user registered
        /// User may belong only to one TENANT
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// Username Or EmailAddress
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Password hash
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
