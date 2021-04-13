using System;

namespace Playground.Core.G1T
{
    public class ErrorDialogResponse : IEquatable<ErrorDialogResponse>
    {

        public ErrorDialogResponse(int response)
        {
            Response = response;
        }

        public static ErrorDialogResponse None { get; } = new ErrorDialogResponse(0);

        public int Response { get; set; }

        public override string ToString()
        {
            if (this.Equals(None))
            {
                return $"{GetType().Name} {nameof(None)}";
            }
            else
            {
                return $"{GetType().Name} {Response}";
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ErrorDialogResponse);
        }

        public bool Equals(ErrorDialogResponse errorDialogResponse)
        {
            if (errorDialogResponse is null) return false;

            return Response == errorDialogResponse.Response;
        }

        public static bool operator ==(ErrorDialogResponse errorDialogResponse1, ErrorDialogResponse errorDialogResponse2)
        {
            return errorDialogResponse1.Equals(errorDialogResponse2);
        }

        public static bool operator !=(ErrorDialogResponse errorDialogResponse1, ErrorDialogResponse errorDialogResponse2)
        {
            return !errorDialogResponse1.Equals(errorDialogResponse2);
        }

        public override int GetHashCode()
        {
            return Response.GetHashCode();
        }
    }
}
