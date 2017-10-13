using SF.Common.Domain.Exceptions;

namespace SF.Common.Help
{
    public class HelpLink
    {
        #region Public Properties

        public byte HelpLinkId { get; private set; }

        public string LinkText { get; private set; }

        public byte Order { get; private set; }

        public string Url { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public HelpLink(
            string url,
            string linkText,
            byte order)
            : this()
        {
            if (string.IsNullOrWhiteSpace(url))
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

        #endregion Public Constructors

        #region Private Constructors

        private HelpLink()
        {
            HelpLinkId = 0;
        }

        #endregion Private Constructors
    }
}