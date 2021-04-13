using System;

namespace Playground.Core.G1T
{
    public class SimpleDialogResponse : IEquatable<SimpleDialogResponse>
    {

        public SimpleDialogResponse(int response)
        {
            Response = response;
        }

        public static SimpleDialogResponse Yes { get; } = new SimpleDialogResponse(0);
        public static SimpleDialogResponse No { get; } = new SimpleDialogResponse(1);
        public static SimpleDialogResponse Confirm { get; } = new SimpleDialogResponse(2);
        public static SimpleDialogResponse Cancel { get; } = new SimpleDialogResponse(3);
        public static SimpleDialogResponse OK { get; } = new SimpleDialogResponse(4);

        public int Response { get; set; }

        public override string ToString()
        {
            string value;
            if (this.Equals(Yes))
            {
                value = nameof(Yes);
            }
            if (this.Equals(No))
            {
                value = nameof(No);
            }
            if (this.Equals(Confirm))
            {
                value = nameof(Confirm);
            }
            if (this.Equals(Cancel))
            {
                value = nameof(Cancel);
            }
            if (this.Equals(OK))
            {
                value = nameof(OK);
            }
            else
            {
                return value = Response.ToString();
            }
            return $"{GetType().Name} {value}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SimpleDialogResponse);
        }

        public bool Equals(SimpleDialogResponse simpleDialogResponse)
        {
            if (simpleDialogResponse is null) return false;

            return Response == simpleDialogResponse.Response;
        }

        public static bool operator ==(SimpleDialogResponse simpleDialogResponse1, SimpleDialogResponse simpleDialogResponse2)
        {
            return simpleDialogResponse1.Equals(simpleDialogResponse2);
        }

        public static bool operator !=(SimpleDialogResponse simpleDialogResponse1, SimpleDialogResponse simpleDialogResponse2)
        {
            return !simpleDialogResponse1.Equals(simpleDialogResponse2);
        }

        public override int GetHashCode()
        {
            return Response.GetHashCode();
        }
    }
}
