using SF.Common.Domain.Exceptions;

namespace SF.Common.Help
{
    public class HelpLink
    {
        public byte HelpLinkId { get; private set; }

        public string LinkText { get; private set; }

        public byte Order { get; private set; }

        public string Url { get; private set; }

        private HelpLink()
        {
            HelpLinkId = 0;
        }

        public HelpLink(
            string url,
            string linkText,
            byte order)
            : this()
        {
            if(string.IsNullOrWhiteSpace(url))
            {
                throw new DomainValidationException(nameof(url), " Url can not be null or empty.");
            }

            if (string.IsNullOrWhiteSpace(linkText))
            {
                throw new DomainValidationException(nameof(linkText), "Link Text can not be null or empty.");
            }

            Url = url;
            LinkText = linkText;
            Order = order;
        }
    }
}