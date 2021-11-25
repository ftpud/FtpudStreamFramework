namespace StreamControlLite.Target
{
    public class StreamPreviewRtmpTarget : StreamTarget
    {
        
        public override string ResolveTarget()
        {
            return "rtmp://192.168.0.129/test";
        }
    }
}