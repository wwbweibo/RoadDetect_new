using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using org.apache.utils;
using org.apache.zookeeper;
using org.apache.zookeeper.data;
using Wwbweibo.CrackDetect.Tools.String;
using ZooKeeperNetEx;

namespace Wwbweibo.CrackDetect.Zookeeper
{
    public class ZookeeperClient
    {
        private static ZooKeeper zkClient;
        private static ConnectionStatusWatcher watcher;
        // 10 秒的超时时间
        private static readonly int zkTimeOut = 10 * 1000;
        private static string connectionString;
        private static long sessionId;
        
        private ZookeeperClient(string[] hosts, string[] ports)
        {
            if ((hosts.Length != ports.Length) || (hosts.Length <= 0 && ports.Length <= 0))
            {
                throw new ApplicationException("the length of hosts and ports must same and must bigger than 0");
            }

            connectionString = "";
            for (var i = 0; i < hosts.Length; i++)
            {
                connectionString += hosts[i] + ":" + ports[i] + ",";
            }

            watcher = new ConnectionStatusWatcher(this);
            connectionString = connectionString.Substring(0, connectionString.Length - 1);
            zkClient = new ZooKeeper(connectionString, zkTimeOut, watcher);
            sessionId = zkClient.getSessionId();

        }

        /// <summary>
        /// 使用给定的主机和端口初始化连接
        /// </summary>
        /// <param name="hosts"></param>
        /// <param name="ports"></param>
        public static ZookeeperClient InitClientConnection(string[] hosts, string[] ports)
        {
            return new ZookeeperClient(hosts, ports);
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        public void InitClientConnection()
        {
            if (connectionString.IsNullOrEmpty())
            {
                return;
            }
            zkClient = new ZooKeeper(connectionString, zkTimeOut, watcher);
            sessionId = zkClient.getSessionId();
        }

        /// <summary>
        /// 请求一个任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="taskId">任务ID</param>
        /// <returns>是否请求成功</returns>
        public bool RequireTask(string taskType, string taskId, string serviceId)
        {
            try
            {
                var taskTodoPath = $"{taskType}/todo/{taskId}";
                var path = $"/{taskType}/inprogress/{taskId}";
                // 待办路径不存在，请求任务失败
                if (zkClient.existsAsync(taskTodoPath).GetAwaiter().GetResult() == null)
                {
                    return false;
                }

                if (zkClient.existsAsync(path).GetAwaiter().GetResult() == null)
                {
                    ensurePath(path);
                    zkClient.createAsync(path, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL).GetAwaiter().GetResult();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 创建一个任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool CreateTask(string taskType, string taskId)
        {
            try
            {
                var taskPath = $"/{taskType}/todo/{taskId}";
                if (zkClient.existsAsync(taskPath).GetAwaiter().GetResult() == null)
                {
                    ensurePath(taskPath);
                    zkClient.createAsync(taskPath, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
                    return true;
                }
            }
            catch(Exception e)
            {
                return false;
            }

            return false;
        }
        
        /// <summary>
        /// 完成一个任务
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public void FinishTask(string taskType, string taskId)
        {
            var todoPath = $"/{taskType}/todo/{taskId}";
            var inprogressPath = $"/{taskType}/inprogress/{taskId}";
            zkClient.deleteAsync(todoPath);
            zkClient.deleteAsync(inprogressPath);
        }

        public void Disconnect()
        {
            zkClient.closeAsync();
            zkClient = null;
        }

        public List<string> ListChildren(string path)
        {
            try
            {
                return zkClient.getChildrenAsync(path).Result.Children;
            }
            catch (Exception e)
            {
                return new List<string>();
            }
        }

        public void RegisterService(string serviceType, Guid serviceId)
        {
            var path = $"/{serviceType}/{serviceId.ToString()}";
            ensurePath(path);
            zkClient.createAsync(path, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL).GetAwaiter()
                .GetResult();
        }

        /// <summary>
        /// 确保给定的路径存在, 如果不存在将会创建节点
        /// </summary>
        /// <param name="path"></param>
        private void ensurePath(string path)
        {
            var nodes = path.Split('/');
            nodes = nodes.Take(nodes.Length - 1).ToArray();
            nodes[0] = "/";
            var cpath = "";
            for (var i = 0; i < nodes.Length; i++)
            {
                cpath = string.Join("/", nodes.Take(i + 1));
                cpath = cpath.Replace("//", "/");
                var exist = zkClient.existsAsync(cpath).GetAwaiter().GetResult() != null;
                if (!exist)
                {
                    zkClient.createAsync(cpath, null, ZooDefs.Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
                }
            }
        }
    }
}
