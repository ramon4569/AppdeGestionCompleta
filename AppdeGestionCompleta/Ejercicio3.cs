using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppdeGestionCompleta
{
    #region Ejercicio 3: Repositorio Genérico (IIdentificable, Repositorio<T>)

    public interface IIdentificable { string Identificacion { get; } }

    public class Repositorio<T> where T : IIdentificable
    {
        private Dictionary<string, T> _elementos = new Dictionary<string, T>();

        public void Agregar(T item)
        {
            if (_elementos.ContainsKey(item.Identificacion)) throw new ArgumentException($"ID duplicado: {item.Identificacion}");
            _elementos.Add(item.Identificacion, item);
            Logger.Log($"Elemento agregado al repositorio {typeof(T).Name}: {item.Identificacion}"); // <-- BONUS LOG
        }

        public T BuscarPorId(string id)
        {
            _elementos.TryGetValue(id, out T item);
            return item;
        }

        public bool Eliminar(string id)
        {
            bool eliminado = _elementos.Remove(id);
            if (eliminado) Logger.Log($"Elemento eliminado del repositorio {typeof(T).Name}: {id}"); // <-- BONUS LOG
            return eliminado;
        }

        public IEnumerable<T> ObtenerTodos() => _elementos.Values;

        public IEnumerable<T> Buscar(Func<T, bool> predicado) => _elementos.Values.Where(predicado);
    }
    #endregion
}
