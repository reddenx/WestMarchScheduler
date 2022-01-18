using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WestMarchSite.Infrastructure;

namespace WestMarchSite.Core
{
    public class SessionEntity
    {
        public SessionStates SessionState { get; private set; }

        public string HostKey { get; }
        public string LeadKey { get; }
        public string PlayerKey { get; }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public string HostName { get; private set; }
        public SessionSchedule HostSchedule { get; private set; }

        public string LeadName { get; private set; }
        public SessionSchedule LeadSchedule { get; private set; }

        public IEnumerable<Player> Players => _playerList;
        private List<Player> _playerList;

        public SessionSchedule FinalizedSchedule { get; private set; }

        public bool IsValid => !_validationErrors.Any();

        private List<string> _validationErrors;

        //for new entities
        public SessionEntity()
            : this(SessionEntity.GenerateHostKey(), SessionEntity.GenerateLeadKey(), SessionEntity.GeneratePlayerKey())
        { }

        //for existing entities prepping for hydration, TODO: protect better, larger pattern at this point would be silly, expand later
        public SessionEntity(string hostKey, string leadKey, string playerKey)
        {
            this.HostKey = hostKey;
            this.LeadKey = leadKey;
            this.PlayerKey = playerKey;

            SessionState = SessionStates.Created;
            _validationErrors = new List<string>();
            _playerList = new List<Player>();
        }

        private static string GeneratePlayerKey()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        private static string GenerateLeadKey()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        private static string GenerateHostKey()
        {
            return Guid.NewGuid().ToString("N").ToLower();
        }

        public void ProgressState()
        {
            switch (this.SessionState)
            {
                case SessionStates.Created:
                    //gotta have all the info filled in before submission
                    if (string.IsNullOrWhiteSpace(this.LeadName))
                        this._validationErrors.Add("lead must be set to move to seek approval");
                    else
                    {
                        this.SessionState = SessionStates.UnApproved;
                    }
                    return;
                case SessionStates.UnApproved:
                    //gotta have the host and host's schedule filled in for review
                    if (string.IsNullOrWhiteSpace(this.HostName) || (!this.HostSchedule?.Options?.Any() ?? true))
                        this._validationErrors.Add("host and their schedule must be set to be approved");
                    else
                    {
                        this.SessionState = SessionStates.Approved;
                    }
                    return;
                case SessionStates.Approved:
                    //lead must submit their schedule, then it should be good to go
                    if (!this.LeadSchedule?.Options?.Any() ?? true)
                        this._validationErrors.Add("lead must have submitted their schedule to move to open");
                    else
                    {
                        this.SessionState = SessionStates.Open;
                    }
                    return;
                case SessionStates.Open:
                    //a final schedule must be set in order for the session to be finalized
                    if (!this.FinalizedSchedule?.Options?.Any() ?? true)
                        this._validationErrors.Add("finalzied schedule must be provided before session can be closed");
                    else
                    {
                        this.SessionState = SessionStates.Finalized;
                    }
                    return;
                case SessionStates.Finalized:
                    this._validationErrors.Add("cannot progress beyond closed state");
                    return;
            }
        }

        public void SetInfo(string title, string description)
        {
            if (!string.IsNullOrWhiteSpace(this.Title))
                this._validationErrors.Add("title already populated");
            else if (!string.IsNullOrWhiteSpace(this.Description))
                this._validationErrors.Add("description already populated");
            else if (string.IsNullOrWhiteSpace(title))
                this._validationErrors.Add("title must not be empty");
            else if (string.IsNullOrWhiteSpace(description))
                this._validationErrors.Add("description must not be empty");
            else
            {
                this.Title = title;
                this.Description = description;
            }
        }

        public void SetLead(string leadName)
        {
            if (!string.IsNullOrWhiteSpace(this.LeadName))
                this._validationErrors.Add("lead is already set");
            else if (string.IsNullOrWhiteSpace(leadName))
                this._validationErrors.Add("lead's name must not be blank");
            else
            {
                this.LeadName = leadName;
            }
        }

        public void SetHost(string hostName)
        {
            if (!string.IsNullOrWhiteSpace(this.HostName))
                this._validationErrors.Add("host is already set");
            else if (string.IsNullOrWhiteSpace(hostName))
                this._validationErrors.Add("hostname can't be empty");
            else
            {
                this.HostName = hostName;
            }
        }

        public void SetHostSchedule(SessionSchedule schedule)
        {
            if (schedule?.Options?.Any() != true)
                this._validationErrors.Add("host schedule must be populated");
            else
            {
                //can't think of any validation errors? maybe state once it's set
                this.HostSchedule = schedule;
            }
        }

        public void SetLeadSchedule(SessionSchedule schedule)
        {
            if (schedule?.Options?.Any() != true)
                this._validationErrors.Add("host schedule must be populated");
            else
            {
                this.LeadSchedule = schedule;
            }
        }

        public void AddPlayer(string name, SessionSchedule schedule)
        {
            if (string.IsNullOrWhiteSpace(name))
                this._validationErrors.Add("player name must be populated");
            else if (schedule?.Options?.Any() != true)
                this._validationErrors.Add("host schedule must be populated");
            else
            {
                this._playerList.Add(new Player(name, schedule));
            }
        }

        public void SetFinalSchedule(SessionSchedule schedule)
        {
            this.FinalizedSchedule = schedule;
        }
    }

    public enum SessionStates
    {
        Created,
        UnApproved,
        Approved,
        Open,
        Finalized
    }
}
