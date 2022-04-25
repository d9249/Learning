import {
    fetchNewsList,
    fetchJobsList,
    fetchAskList,
    fetchUserInfo,
    fetchCommentItem,
    fetchList
} from '../api/index.js'

export default {
    // promise
    // FETCH_NEWS(context) {
    //     return fetchNewsList()
    //         .then(response => {
    //             context.commit('SET_NEWS', response.data);
    //             return response
    //         })
    //         .catch(error => {
    //             console.log(error);
    //         })
    // },
    // async
    async FETCH_NEWS(context) {
        const response = await fetchNewsList();
        context.commit('SET_NEWS', response.data);
        return response;
        // 어떠한 값을 return 하더라도 Promise가 return 된다.
    },
    // promise
    // FETCH_JOBS({
    //     commit
    // }) {
    //     return fetchJobsList()
    //         .then(({
    //             data
    //         }) => {
    //             commit('SET_JOBS', data);
    //         })
    //         .catch(error => {
    //             console.log(error);
    //         })
    // },
    // async
    async FETCH_JOBS({
        commit
    }) {
        try {
            const response = await fetchJobsList();
            commit('SET_JOBS', response.data);
            return response;
        } catch (error) {
            console.log(error);
        }
    },
    // Promise
    // FETCH_ASK({
    //     commit
    // }) {
    //     return fetchAskList()
    //         .then(({
    //             data
    //         }) => {
    //             commit('SET_ASK', data);
    //         })
    //         .catch(error => {
    //             console.log(error);
    //         })
    // },
    // async
    async FETCH_ASK({
        commit
    }) {
        const response = await fetchAskList();
        commit('SET_ASK', response.data);
        return response
    },
    //primise
    // FETCH_USER({
    //     commit
    // }, name) {
    //     return fetchUserInfo(name)
    //         .then(({
    //             data
    //         }) => {
    //             commit('SET_USER', data);
    //         })
    //         .catch(error => {
    //             console.log(error);
    //         })
    // },
    //async
    async FETCH_USER({
        commit
    }, name) {
        try {
            const response = await fetchUserInfo(name);
            commit('SET_USER', response.data)
            return response;
        } catch (error) {
            console.log(error);
        }
    },
    // Promise
    // FETCH_ITEM({
    //     commit
    // }, id) {
    //     return fetchCommentItem(id)
    //         .then(({
    //             data
    //         }) => {
    //             commit('SET_ITEM', data);
    //         })
    //         .catch(error => {
    //             console.log(error);
    //         })
    // },
    //async
    async FETCH_ITEM({
        commit
    }, id) {
        try {
            const response = await fetchCommentItem(id);
            commit('SET_ITEM', response.data);
            return response;
        } catch (error) {
            console.log(error);
        }
    },
    // promise
    // FETCH_LIST({
    //     commit
    // }, pageName) {
    //     return fetchList(pageName)
    //         .then(
    //             response => {
    //                 // console.log(4);
    //                 commit('SET_LIST', response.data);
    //                 return response;
    //             })
    //         .catch(error => console.log(error));
    // },
    async FETCH_LIST({
        commit
    }, pageName) {
        const response = await fetchList(pageName);
        commit('SET_LIST', response.data);
        return response;
    }
}