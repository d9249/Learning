import Vue from "vue";
import VueRouter from "vue-router";

import MainView from "../views/MainView";
import SigninView from "../views/SigninView";
import SignupView from "../views/SignupView";
import MyPageView from "../views/MyPageView";
import IntroductionView from "../views/IntroductionView";
import PublicationView from "../views/PublicationView";
import GalleryView from "../views/GalleryView";
import NotFoundView from "../views/NotFoundView";
import BoardView from "../views/BoardView";
import PostView from "../views/PostView";
import LectureView from "../views/LectureView";
import ContactView from "../views/ContactView";
import PostWriteView from "../views/PostWriteView";
import PostEditView from "../views/PostEditView";
import ForgottenPasswordView from "../views/ForgottenPasswordView";
import ChangePasswordView from "../views/ChangePasswordView";
import MyPageBeforeCheckView from "../views/MyPageBeforeCheckView";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history",
  routes: [
    {
      path: "/", // url 주소
      component: MainView, // 표시될 컴포넌트(페이지)
    },
    {
      path: "/signin",
      component: SigninView,
    },
    {
      path: "/signup",
      component: SignupView,
    },
    {
      path: "/mypage",
      component: MyPageView,
    },
    {
      path: "/mypage-before",
      component: MyPageBeforeCheckView,
    },
    {
      path: "/intro",
      component: IntroductionView,
    },
    {
      path: "/publication",
      component: PublicationView,
    },
    {
      path: "/lecture",
      component: LectureView,
    },
    {
      path: "/lecture/:lectureId",
      component: BoardView,
      props: true,
    },
    {
      path: "/lecture/:lectureId/post/write",
      component: PostWriteView,
    },
    {
      path: "/lecture/:lectureId/post/:postId",
      component: PostView,
      props: true,
    },
    {
      path: "/lecture/:lectureId/post/:postId/edit",
      component: PostEditView,
    },
    {
      path: "/gallery",
      component: GalleryView,
    },
    {
      path: "/contact",
      component: ContactView,
    },
    {
      path: "/forgotten",
      component: ForgottenPasswordView,
    },
    {
      path: "/forgotten/change",
      component: ChangePasswordView,
    },
    {
      path: "*",
      component: NotFoundView,
    },
  ],
});

export default router;
