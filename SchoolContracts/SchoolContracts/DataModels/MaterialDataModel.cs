﻿namespace SchoolContracts.DataModels;
public class MaterialDataModel (string id, string storekeeperId, string materialName, string description)
{
    public string Id { get; private set; } = id;
    public string StorekeeperId { get; private set; } = storekeeperId;
    public string MaterialName { get; private set; } = materialName;
    public string Description { get; private set; } = description;
}
