function SearchDate({
  setContaSelect,
  setStartDate,
  setEndDate,
  startDate,
  endDate,
  handleSearch,
  contas = [],
}) {
  return (
    <div className="flex flex-center items-center">
      <div className="flex text-center flex-center">
        <div className="flex mb-4">
          <div className="flex flex-col mr-4">
            <label className="text-start">Contas</label>
            <select
              className="p-2 border border-stone-300 rounded-md focus:none focus:border-primary"
              onChange={(e) => setContaSelect(e.target.value)}
            >
              {contas.map((conta) => (
                <option value={conta.id}>{conta.nome}</option>
              ))}
            </select>
          </div>
          <div className="flex flex-col">
            <label className="text-start">Inicio</label>
            <input
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              className="p-2 border border-stone-300 rounded-md focus:none focus:border-primary "
            />
          </div>
          <span className="ml-2 mr-2 mt-8">A</span>
          <div className="flex flex-col">
            <label className="text-start">Fim</label>
            <input
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              className="p-2 border border-stone-300 rounded-md focus:none focus:border-primary"
            />
          </div>
        </div>
      </div>
      <div className="-mt-4 ml-3">
        <button
          onClick={handleSearch}
          className="py-2 px-4 mt-6 bg-primary text-white rounded-md hover:bg-blue-600 focus:outline-none focus:ring focus:border-blue-300"
        >
          Buscar
        </button>
      </div>
    </div>
  );
}
export default SearchDate;
