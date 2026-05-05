namespace FrontendVitalCare.Adaptadores
{
public interface IAdapter<TOrigen, TDestino>
    {
        TDestino Adapt(TOrigen origen);
        List<TDestino> AdaptList(IEnumerable<TOrigen> origen);
    }
}