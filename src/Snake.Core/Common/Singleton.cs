namespace Snake.Core.Common
{
    public abstract class Singleton<T>
        where T : class, new()
    {
        #region Static Field

        private static T s_Instance;
        private static readonly object s_lock = new object();

        #endregion

        #region Static

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    lock (s_lock)
                    {
                        if (s_Instance == null)
                        {
                            s_Instance = new T();
                        }
                    }
                }

                return s_Instance;
            }
        }


        /// <summary>
        /// Initializes the specified customer cache.
        /// </summary>
        /// <param name="instance">The customer cache.</param>
        public static void Initialize(T instance)
        {
            s_Instance = instance;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            s_Instance = default(T);
        }

        #endregion
    }
}
