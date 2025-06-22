import {ReactNode, useState} from "react";
import {ItemBorder} from "./ItemBorder.tsx";
import {CustomButton} from "./inputs/CustomButton.tsx";
import {Divider} from "./Divider.tsx";

export interface IExpandableItemPanelProps {
  content?: ReactNode,
  expandedContent?: ReactNode;
}

export function ExpandableItemPanel(props: IExpandableItemPanelProps) {
  const {content, expandedContent} = props;

  const [expanded, setExpanded] = useState(false);

  function toggle() {
    setExpanded(value => !value);
  }

  const toggleButtonIcon = expanded
      ? "pi pi-chevron-up"
      : "pi pi-chevron-down";

  return (
      <ItemBorder>
        <div style={{display: "flex", flexDirection: "row", gap: "10px"}}>
          <CustomButton onClick={toggle}>
            <span
                style={{
                  width: "15px",
                  fontSize: "13px",
                  transition: "transform 0.3s ease"
                }}
                className={toggleButtonIcon}/>
          </CustomButton>
          <div style={{flex: 1}}>
            {content}
          </div>
        </div>
        <div style={{
          overflow: "hidden",
          marginLeft: "15px",
          transition: "max-height 0.3s ease, padding 0.4s ease",
          maxHeight: expanded ? "100%" : 0,
          padding: expanded ? "10px" : "0 10px",
          opacity: expanded ? 1 : 0,
          transitionProperty: "max-height, opacity, padding"
        }}>
          <Divider/>
          {expandedContent}
        </div>
      </ItemBorder>
  );
}