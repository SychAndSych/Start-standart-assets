//интерфейс для взаимодействия с объектами
namespace Sych_scripts
{
    public enum Interaction_enum
    {
        None,
        Talking,
        Interaction,
        Take
    }

    interface I_Interaction
    {

        public Interaction_enum Interaction_type { get;}

        void Interaction();

        public string Find_out_type_object
        {
            get
            {
                return Interaction_type.ToString();
            }
        }
    }
}