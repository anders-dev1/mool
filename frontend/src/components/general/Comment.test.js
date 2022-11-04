import renderer from "react-test-renderer";
import {Comment} from "./Comment";
import {testTheme} from "./__test-utilities__/TestTheme";
import {LikeCommentClient} from "../../api";
import {fireEvent, render, screen} from "@testing-library/react";
import {internalsForRenderer, renderWithProviders} from "./__test-utilities__/testRenders";

const likedComment = {
    id: 1,
    authorName: "Person Personsen",
    content: "This is the comment content.",
    likes: 42,
    likedByCurrentUser: true,
    isChangingLikeStatus: false
}

const unlikedComment = {
    id: 1,
    authorName: "This is the comment content.",
    content: "Lorem ipsum",
    likes: 42,
    likedByCurrentUser: false,
    isChangingLikeStatus: false
}

function createState(comment){
    return {
        commentsReducer: {
            commentsById:{
                1: comment
            }
        }
    }
}

beforeEach(() => {
    jest.spyOn(LikeCommentClient.prototype, 'unlike')
        .mockImplementation(() => {
            return Promise.resolve();
        });
    jest.spyOn(LikeCommentClient.prototype, 'like')
        .mockImplementation(() => {
            return Promise.resolve();
        });
});

function renderComment(comment) {
    renderWithProviders(
        <Comment commentId={1}/>,
        {preloadedState: createState(comment)});

    return screen.getByText("👍").parentElement;
}

test('renders correctly', () => {
    const internalDom = internalsForRenderer(
        <Comment commentId={1}/>,
        {preloadedState: createState(unlikedComment)});

    const tree = renderer
        .create(internalDom)
        .toJSON();

    expect(tree).toMatchSnapshot();
});


describe('like button clicked, when comment unliked', () => {
    test('should change to success color', async () => {
        // Arrange
        const likeButton = renderComment(unlikedComment);

        // Act
        fireEvent.click(likeButton);
        await new Promise(process.nextTick);

        // Assert
        expect(likeButton).toHaveStyle(`background-color: ${testTheme.successColor};`);
    });

    test('should increment likes number', async () => {
        // Arrange
        const likeButton = renderComment(unlikedComment);

        // Act
        fireEvent.click(likeButton);
        await new Promise(process.nextTick);

        // Assert
        const number = screen.getByText(unlikedComment.likes + 1);
        expect(number).toBeDefined();
    });
});

describe('like button clicked, when comment liked', () => {
    test('becomes primary color', async () => {
        // Arrange
        const likeButton = renderComment(likedComment);

        // Act
        fireEvent.click(likeButton);
        await new Promise(process.nextTick);

        // Assert
        expect(likeButton).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
    });

    test('should decrement likes number', async () => {
        // Arrange
        const likeButton = renderComment(likedComment);

        // Act
        fireEvent.click(likeButton);
        await new Promise(process.nextTick);

        // Assert
        const number = screen.getByText(likedComment.likes - 1);
        expect(number).toBeDefined();
    });
});


test('clicks on the like button, when changing like status, should not trigger more changes', async () => {
    // Arrange
    const likeButton = renderComment(likedComment);

    // Act
    fireEvent.click(likeButton); // unlike
    fireEvent.click(likeButton); // like
    await new Promise(process.nextTick);

    // Assert
    // The status change was only triggered once making it unliked.
    expect(likeButton).toHaveStyle(`background-color: ${testTheme.primaryColor};`);
});