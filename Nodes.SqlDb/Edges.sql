CREATE TABLE [dbo].[Edges] (
    [Id] BIGINT NOT NULL IDENTITY (1, 1),
    [Version] ROWVERSION,
    [Value] NVARCHAR(MAX) NOT NULL,
    [FromNodeId] BIGINT NOT NULL,
    [ToNodeId] BIGINT NOT NULL,

    CONSTRAINT [PK_dbo.Edges] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Edges_FromNodeId] FOREIGN KEY ([FromNodeId]) REFERENCES [dbo].[Nodes] ([Id]),
    CONSTRAINT [FK_dbo.Edges_ToNodeId] FOREIGN KEY ([ToNodeId]) REFERENCES [dbo].[Nodes] ([Id])
)
