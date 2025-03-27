import "./PasswordCredentialsInputText.css"
import { InputText } from "primereact/inputtext";
import { ChangeEventHandler, HTMLInputTypeAttribute } from "react";

export interface Props {
	placeholder?: string;
	type?: HTMLInputTypeAttribute;

	name?: string,
	value?: string,
	onChange?: ChangeEventHandler<HTMLInputElement> 

	error?: string,
}

export function PasswordCredentialsInputText(props: Props) {
	return (
		<div className="pc-input">
			<InputText 
				className="p-inputtext-sm" 
				type={props.type}
				name={props.name}
				placeholder={props.placeholder} 
				value={props.value} 
				onChange={props.onChange} />
			{props.error !== "" && <p>{props.error}</p>}
		</div>
	);
}