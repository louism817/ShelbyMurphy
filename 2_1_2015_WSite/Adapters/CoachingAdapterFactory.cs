using _2_1_2015_WSite.Adapters.DataAdapters;
using _2_1_2015_WSite.Adapters.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2_1_2015_WSite.Adapters
{
    public static class CoachingAdapterFactory
    {
        public static ICoachingAdapter GetCoachingAdapter()
        {
            return new CoachingDataAdapter();
        }
    }
}