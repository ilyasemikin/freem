import {useEffect, useState} from "react";

export function Clock() {
  const [now, setNow] = useState<Date>(new Date());

  useEffect(() => {
    const timer = setInterval(() => {
      const now = new Date();
      const next = new Date(Date.UTC(now.getFullYear(), now.getMonth(), now.getDate()));
      setNow(next);
    }, 500);

    return () => clearInterval(timer);
  }, []);

  return (
      <span>{now.toString()}</span>
  );
}