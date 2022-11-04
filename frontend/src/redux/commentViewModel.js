export function mapCommentModelToViewModel(comment) {
    return {
        id: comment.id,
        authorName: comment.authorName,
        content: comment.content,
        likes: comment.likes,
        likedByCurrentUser: comment.likedByCurrentUser,
        isChangingLikeStatus: false
    }
}