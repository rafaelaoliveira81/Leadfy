using System.ComponentModel;

namespace Dominio.Enums
{
    public enum AiModelsEnum
    {
        [Description("openai/gpt-4.1")]
        Gpt41 = 1,

        [Description("openai/gpt-4.1-mini")]
        Gpt41Mini = 2,

        [Description("openai/gpt-4o")]
        Gpt4o = 3,

        [Description("openai/gpt-4o-mini")]
        Gpt4oMini = 4,

        [Description("anthropic/claude-3.5-sonnet")]
        Claude35Sonnet = 5,

        [Description("meta/llama-3.3-70b-instruct")]
        Llama3370B = 6,

        [Description("mistral-ai/mistral-large")]
        MistralLarge = 7,

        [Description("mistral-ai/mistral-small")]
        MistralSmall = 8,

        [Description("microsoft/phi-4")]
        Phi4 = 9,

        [Description("deepseek/deepseek-v3")]
        DeepSeekV3 = 10,

        [Description("deepseek/deepseek-r1")]
        DeepSeekR1 = 11
    }
}