-- Script Date: 15/04/2020 10:53  - ErikEJ.SqlCeScripting version 3.5.2.85
CREATE TABLE [IntegrationEventLog] (
  [Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL
, [EventId] bigint NOT NULL
, [Content] text NOT NULL
, [CreationTime] text NOT NULL
, [TimesSent] text NOT NULL
, [EventTypeName] text NOT NULL
, [State] text NULL
);

