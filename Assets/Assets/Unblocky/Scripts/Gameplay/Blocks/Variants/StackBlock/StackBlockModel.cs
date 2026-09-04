using System.Collections.Generic;

namespace Flavor
{
    public class StackBlockModel
    {
        public StackBlockConfig Config;
        public StackBlockRuntime Runtime;

        // ?? ??NH CAO ? ?ÂY: List ch?a các c?c Model nh? ?? thao tác cho l?!
        public List<StackItemModel> ItemModels;
        // Hàm ?? ra Model
        public StackBlockModel(StackBlockConfig config, StackBlockRuntime runtime)
        {
            this.Config = config;
            this.Runtime = runtime;
            this.ItemModels = new List<StackItemModel>();

            // Link t?i nh? l?i v?i nhau
            for (int i = 0; i < config.Items.Count; i++)
            {
                this.ItemModels.Add(new StackItemModel
                {
                    Config = config.Items[i],
                    Runtime = runtime.Items[i]
                });
            }
        }
    }
}