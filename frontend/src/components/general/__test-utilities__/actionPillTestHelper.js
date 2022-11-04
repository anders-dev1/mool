import {screen} from "@testing-library/react";

export function findActionPillByIcon(icon) {
    return screen.getByText(icon).closest('div');
}

export function findActionPillNumberByIcon(icon) {
    const pill = findActionPillByIcon(icon);
    const numberP = pill.lastChild.lastChild;
    return numberP.textContent;
}