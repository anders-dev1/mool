import {render} from "@testing-library/react";
import MainContentArea from "./MainContentArea";

test('renders correctly', () => {
    const {container} = render(
        <MainContentArea><p>This content should be inside the area :)</p></MainContentArea>
    );

    expect(container).toMatchSnapshot();
});