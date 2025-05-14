
public class Material
{
    public ScavengedMaterials scavMat {get; private set;}
    public RawMaterials rawMat {get; private set;}
    public Material(RawMaterials mat){
        this.rawMat = mat;
    }

    public Material(ScavengedMaterials mat){
        this.scavMat = mat;
    }
    /// <summary>
    /// Raw materials, more based <para />
    /// <example> 
    /// iron legs provide heavy stability, gold/copper used for slightly electrical components (weak targeting, not as smart as computeunit), Steel used for some sweet large calibre rounds ? 
    /// </example>
    /// </summary>
    public enum RawMaterials
    {
        Steel,
        Copper,
        Aluminum,
        Iron,
        Gold,
    }

    /// <summary>
    /// Rarer drops (?) from enemy units <br /> 
    /// Used for more complicated behaviour in turret parts
    /// <para /> 
    /// For Example: <br />
    /// <example> 
    /// Smart artillery would use Compute, sensor array, 
    /// to "sense and compute" firing path for most damage (hit big collection of enemies)
    /// </example> <para /> 
    /// <remarks>
    /// None of these have to be used right now, as these sort of imply more complicated behaviour to be implemented
    /// </remarks>
    /// </summary>
    public enum ScavengedMaterials
    {
        ComputeUnit,
        Arm,
        Leg,
        SensorArray,
        Core,
        ReactiveArmorPlating,
    }
}