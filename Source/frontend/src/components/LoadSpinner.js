function LoadSpinner({ isLoading }) {
  return (
    <div>
      {isLoading && (
        <div className="absolute inset-0 flex items-center justify-center bg-white bg-opacity-75">
          <div className="animate-spin h-6 w-6 border-t-2 text-primary border-solid rounded-full"></div>
        </div>
      )}
    </div>
  );
}

export default LoadSpinner;
