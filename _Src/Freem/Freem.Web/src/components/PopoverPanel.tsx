import {forwardRef, ReactNode, useEffect, useImperativeHandle, useLayoutEffect, useRef, useState} from "react";

export interface IPopoverPanelHandle {
  toggle: (target: HTMLElement | EventTarget) => void;
  hide: () => void;
}

export interface IPopoverPanelProps {
  children?: ReactNode;
}

class PopoverPosition {
  public readonly top: number;
  public readonly left: number;

  public constructor(top: number = 0, left: number = 0) {
    this.top = top;
    this.left = left;
  }
}

export const PopoverPanel = forwardRef<IPopoverPanelHandle, IPopoverPanelProps>((props, ref) => {
  const {children} = props;

  const [visible, setVisible] = useState(false);
  const [position, setPosition] = useState<PopoverPosition>(new PopoverPosition());
  const [node, setNode] = useState<HTMLElement | null>(null);

  const panelRef = useRef<HTMLDivElement>(null);

  useLayoutEffect(() => {
    const panel = panelRef.current;
    if (!visible || !panel) {
      return;
    }

    function fit() {
      const margin = 8;
      const width = panel!.offsetWidth;
      const minLeft = window.scrollX + margin;
      const maxLeft = window.scrollX + document.documentElement.clientWidth - width - margin;
      const left = Math.max(minLeft, Math.min(position.left, maxLeft));

      if (left !== position.left) {
        setPosition(new PopoverPosition(position.top, left));
      }
    }

    fit();

    const observer = new ResizeObserver(fit);
    observer.observe(panel);

    return () => {
      observer.disconnect();
    };
  }, [visible, position]);

  useEffect(() => {
    function click(e: MouseEvent) {
      const target = e.target as Node;
      if (panelRef.current && !panelRef.current.contains(target) && !node?.contains(target)) {
        setVisible(false);
      }
    }

    if (visible) {
      document.addEventListener("mousedown", click);
    }

    return () => {
      document.removeEventListener("mousedown", click);
    }
  }, [node, visible]);

  useImperativeHandle(ref, () => ({
    toggle(target: HTMLElement | EventTarget) {
      if (target instanceof HTMLElement) {
        const rect = target.getBoundingClientRect();

        const position = new PopoverPosition(rect.bottom + window.scrollY + 4, rect.left + window.scrollX);
        setPosition(position);
        setNode(target)
        setVisible(value => !value);
      }
    },
    hide() {
      setVisible(false);
    }
  }));

  return (
      <>
        {visible && <div
            ref={panelRef}
            style={{
              position: "absolute",
              width: "max-content",
              top: position.top,
              left: position.left,
              zIndex: 1000
            }}>
          {children}
        </div>}
      </>
  );
});