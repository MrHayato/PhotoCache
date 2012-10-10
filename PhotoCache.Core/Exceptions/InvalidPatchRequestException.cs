using System;
using PhotoCache.Core.Services;

namespace PhotoCache.Core.Exceptions
{
    public class InvalidPatchRequestException : Exception
    {
        public ModelPatch[] Patches { get; set; }
    }
}
