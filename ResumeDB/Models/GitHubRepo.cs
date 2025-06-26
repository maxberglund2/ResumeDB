﻿using System.Text.Json.Serialization;

public class GitHubRepo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("html_url")]
    public string HtmlUrl { get; set; }
}
