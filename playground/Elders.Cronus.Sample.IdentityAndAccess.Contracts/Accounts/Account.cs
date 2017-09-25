using Elders.Cronus.DomainModeling;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts;
using Elders.Cronus.Sample.IdentityAndAccess.Accounts.Events;
using System;

namespace Elders.Cronus.Sample.IdentityAndAccess.Contracts.Accounts
{
    public class Account : AggregateRoot<AccountState>
    {
        Account() { }

        public Account(AccountId userId, string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentException(nameof(email));

            var evnt = new AccountRegistered(userId, email);
            state = new AccountState();
            Apply(evnt);
        }

        public void ChangeEmail(string oldEmail, string newEmail)
        {
            if (string.IsNullOrEmpty(oldEmail)) throw new ArgumentException(nameof(oldEmail));
            if (string.IsNullOrEmpty(newEmail)) throw new ArgumentException(nameof(newEmail));

            if (oldEmail != newEmail)
            {
                var evnt = new AccountEmailChanged(state.Id, oldEmail, newEmail);
                Apply(evnt);
            }
        }
    }
}
