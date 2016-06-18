using Orchard.UI.Resources;

namespace XinTuo.Finance
{
    public class FinanceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            ResourceManifest manifest = builder.Add();

            //easyui全部控件样式
            manifest.DefineStyle("Easyui").SetUrl("default/easyui.css");
            //自定义的主样式
            manifest.DefineStyle("MainSty").SetUrl("Styles.css").SetDependencies("Easyui");
            //凭证样式
            manifest.DefineStyle("voucher").SetUrl("voucher.css").SetDependencies("Easyui");

            //easyui库
            manifest.DefineScript("Easyui").SetUrl("jquery.easyui.min.js").SetDependencies("jQuery");
            //easyui中文化
            manifest.DefineScript("Easyui.cn").SetUrl("easyui-lang-zh_CN.js").SetDependencies("Easyui");
            //easyui验证方法扩展
            manifest.DefineScript("Easyui.Extend").SetUrl("easyui.extend.js").SetDependencies("Easyui");

            //企业信息相关脚本
            manifest.DefineScript("Company.App").SetUrl("App/Company.App.js").SetDependencies("Easyui.cn", "Easyui.Extend");

            //科目设置
            manifest.DefineScript("Subject.App").SetUrl("App/Subject.App.js").SetDependencies("Easyui.cn", "Easyui.Extend");

            //凭证录入、查询
            manifest.DefineScript("Voucher.App").SetUrl("App/Voucher.App.js").SetDependencies("Easyui.cn", "Easyui.Extend");
        }
    }
}