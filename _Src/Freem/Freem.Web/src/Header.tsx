import {useAuthorization} from "./contexts/authorization/useAuthorization.ts";

export function Header() {
  const {user} = useAuthorization();
  if (!user)
    throw new Error("Unauthorized");

  return (
    <div style={{display: "flex", justifyContent: "end", width: "100%", padding: "10px 20px"}}>
      <span style={{cursor: "pointer"}}>{user.nickname}</span>
    </div>
  );
}