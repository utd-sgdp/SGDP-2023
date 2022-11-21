using UnityEngine;

namespace Game
{
    /// <summary>
    /// <para>
    /// Contains similar data to a <see cref="Transform"/>. This is useful for events or other mechanisms that would
    /// need to pass data without creating a <see cref="Transform"/> in the world.
    /// </para>
    /// <para>
    /// Implicit data cast from <see cref="Transform"/> to <see cref="TransformData"/> makes it easy to use. 
    /// </para>
    /// </summary>
    public class TransformData
    {
        public Vector3 Position = Vector3.zero;
        public Quaternion Rotation = Quaternion.identity;
        public Vector3 Scale = Vector3.one;
        
        public Vector3 LocalPosition = Vector3.zero;
        public Quaternion LocalRotation = Quaternion.identity;
        public Vector3 LocalScale = Vector3.one;
        

        public TransformData() { }
        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
        }
        
        public TransformData(Vector3 position, Quaternion rotation, Vector3 scale, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            LocalScale = localScale;
        }

        public static implicit operator TransformData(Transform trans)
        {
            return new TransformData(
                trans.position, trans.rotation, trans.lossyScale,
                trans.localPosition, trans.localRotation, trans.localScale
            );
        }

        public override string ToString()
        {
            return $"Position: {Position}\n" +
                   $"Rotation: {Rotation}\n" +
                   $"Scale: {Scale}\n" +
                   $"LocalPosition: {LocalPosition}\n" +
                   $"LocalRotation: {LocalRotation}\n" +
                   $"LocalScale: {LocalScale}\n";
        }
    }
}