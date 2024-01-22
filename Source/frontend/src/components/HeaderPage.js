function HeaderPage({ title, description }) {
  return (
    <div className="mb-8">
      <h2 className="text-2xl font-semibold   text-gray">{title}</h2>
      <span className="text-slate-400">{description}</span>
    </div>
  );
}

export default HeaderPage;
