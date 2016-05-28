using System;
using System.Collections.Generic;
using System.Web.Http;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace XinTuo.Finance
{
    public class ApiRoutes : IHttpRouteProvider
    {
        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new RouteDescriptor[] {
                new HttpRouteDescriptor {
                    Name="Company Api",
                    Priority=0,
                    RouteTemplate="api/c/{action}/{id}",
                    Defaults=new {
                        area="XinTuo.Finance",
                        controller="CompanyApi",
                        id=RouteParameter.Optional
                    }
                },
                new HttpRouteDescriptor {
                    Name="Subject Setting Api",
                    Priority=0,
                    RouteTemplate="api/s/{action}/{id}",
                    Defaults=new {
                        area="XinTuo.Finance",
                        controller="SubjectApi",
                        id=RouteParameter.Optional
                    }
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach(RouteDescriptor rd in GetRoutes())
            {
                routes.Add(rd);
            }
        }
    }
}