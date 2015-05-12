namespace SkipList.Core
{
    internal static class ThrowHelper
    {
        internal static void ThrowKeyNotFoundException()
        {
            throw new System.Collections.Generic.KeyNotFoundException();
        }

        internal static void ThrowInvalidOperationException()
        {
            throw new System.InvalidOperationException();
        }

        internal static void ThrowInvalidOperationException_EnumFailedVersion()
        {
            throw new System.InvalidOperationException("Collection was modified after the enumerator was instantiated.");
        }
    }
}